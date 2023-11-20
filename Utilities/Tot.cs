using System.Collections.Concurrent;
using OllamaTot.Models;
using OllamaTot.Dtos;
using System.Text.RegularExpressions;

namespace OllamaTot.Utilities
{
  public class Tot
  {
    private readonly ILogger<Tot> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly OllamaTotContext _context;

    private readonly Node _root;

    private readonly int _numLeaves;
    private readonly int _maxDepth;

    private string? _prompt;
    private readonly string _proposePrompt;
    private readonly string _evaluatePrompt;

    private OllamaRequest _ollamaRequest;

    private readonly List<string> _serverList;
    private readonly ServerQueue _queue;

    public Tot(ILogger<Tot> logger, IHttpClientFactory clientFactory, OllamaTotContext context)
    {
      _logger = logger;
      _clientFactory = clientFactory;
      _context = context;

      _root = new Node(0);

      // defaults
      _numLeaves = 3;
      _maxDepth = 6;

      _ollamaRequest = new OllamaRequest("openhermes2.5-mistral");

      _proposePrompt = FileHelper.ReadPromptFile("Propose.txt");
      _evaluatePrompt = FileHelper.ReadPromptFile("Evaluate.txt");

      _serverList = new List<string>() { "localhost", "192.168.0.112" };
      _queue = new ServerQueue(_serverList);
    }

    public async Task Propose(Node node, HttpClient client)
    {
      // clone base request, override default and user-provided options
      var request = new OllamaRequest(_ollamaRequest);
      request.Options.Seed = node.Seed;
      request.Options.Stop = new string[] { (node.Steps.Count + 2).ToString() };

      // build and add prompt
      var prompt = string.Format(_proposePrompt, _prompt, node);
      request.Prompt = prompt;

      var ollama = new Ollama(client, request);

      // threadsafe server access
      var serverIP = await _queue.Wait();
      _logger.LogInformation($"Thread {node.Seed} Propose Got Server IP: {serverIP}");
      var res = await ollama.Generate(serverIP);
      _queue.Release(serverIP);

      node.Steps.Add(res);
    }

    public async Task Evaluate(Node node, HttpClient client)
    {
      var request = new OllamaRequest(_ollamaRequest);
      request.Options.Seed = node.Seed;
      request.Options.Stop = new string[] { (node.Steps.Count + 2).ToString() };

      var prompt = string.Format(_evaluatePrompt, _prompt, node);
      request.Prompt = prompt;

      var ollama = new Ollama(client, request);

      // multiple retries in case model refuses to generate a numerical score
      while (true)
      {
        var serverIP = await _queue.Wait();
        _logger.LogInformation($"Thread {node.Seed} Evaluate Got Server IP: {serverIP}");
        var res = await ollama.Generate(serverIP);
        _queue.Release(serverIP);

        string pattern = @"[0-9]+";
        Match match = Regex.Match(res, pattern);

        int number;
        if (match.Success && int.TryParse(match.Value, out number))
        {
          node.Score = number;
          return;
        }
        else
        {
          _logger.LogWarning($"Thread {node.Seed} Failed to parse score from response: {res}");
        }
      }
    }

    public async Task Spawn(Node parent)
    {
      var tasks = new List<Task>();
      for (var i = 0; i < _numLeaves; i++)
      {
        var client = _clientFactory.CreateClient();
        var node = new Node(parent, i);
        parent.Children.Add(node);

        tasks.Add(Task.Run(() => Propose(node, client))
          .ContinueWith(async task => await Evaluate(node, client))
          .Unwrap());
      }
      await Task.WhenAll(tasks);
    }

    public async Task<string> Run(string prompt, OllamaRequest? ollamaRequest = null)
    {
      _prompt = prompt;
      _ollamaRequest = ollamaRequest ?? _ollamaRequest;
      var current = _root;
      var depth = 0;

      while (depth < _maxDepth)
      {
        await Spawn(current);

        var topScore = 0;
        foreach (var child in current.Children)
        {
          // currently evaluator model is too unreliable to actually set this to the max score
          // in the prompt, so this is more of a placeholder before turning it into a constant
          if (child.Score == 100)
          {
            current = child;
            break;
          }

          // shouldn't happen, but effectively eliminates this branch
          var score = child.Score ?? 0;

          if (score > topScore)
          {
            topScore = score;
            current = child;
          }
        }
        depth++;
        _logger.LogInformation($"Depth: {depth}");
      }
      _logger.LogInformation($"Final: {current}");
      return current.ToString();
    }
  }

  public class Node
  {
    public int Seed { get; set; }
    public List<Node> Children { get; set; }
    public List<string> Steps { get; set; }
    public int? Score { get; set; }

    public Node(int seed)
    {
      Children = new List<Node>();
      Steps = new List<string>();
      Seed = seed;
    }

    public Node(Node parent, int seed)
    {
      Children = new List<Node>();
      Steps = new List<string>(parent.Steps);
      Seed = seed;
    }

    public override string ToString()
    {
      return string.Join(Environment.NewLine, Steps);
    }
  }


  public class ServerQueue
  {
    private readonly SemaphoreSlim _semaphore;
    private readonly ConcurrentQueue<string> _queue;
    private readonly int _maxConcurrent;

    public ServerQueue(IEnumerable<string> serverList)
    {
      _semaphore = new SemaphoreSlim(serverList.Count(), serverList.Count());
      _queue = new ConcurrentQueue<string>(serverList);
      _maxConcurrent = serverList.Count();
    }

    public async Task<string> Wait()
    {
      await _semaphore.WaitAsync();
      if (_queue.TryDequeue(out var serverIP))
      {
        return serverIP;
      }
      // should never happen
      throw new Exception("No servers available");
    }

    public void Release(string serverIP)
    {
      _queue.Enqueue(serverIP);
      _semaphore.Release();
    }
  }
}
