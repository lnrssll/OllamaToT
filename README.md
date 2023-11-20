# Ollama Tree of Thoughts (ToT)

This project is a basic implementation of the [Tree of Thoughts](https://arxiv.org/abs/2305.10601) LLM agent prompting system, using asynchronous multithreading to coordinate work by multiple networked machines.

## Inference

The actual LLM inference is performed using the [OpenHermes2.5-Mistral](https://ollama.ai/library/openhermes2.5-mistral) 7B model (quantized q4_0), running on [Ollama](ollama.ai). I use Ollama, which uses [llama.cpp](https://github.com/ggerganov/llama.cpp) under the hood, because it comes with a built in Go server that provides ready-made [API endpoints](https://github.com/jmorganca/ollama/blob/main/docs/api.md#generate-a-completion) for basic LLM tasks. I have two GPU-equipped machines in my house that are capable of running LLM inference at respectable speeds, so I startup the Ollama server on both machines and modify the [firewall defaults](https://github.com/jmorganca/ollama/blob/main/docs/faq.md) to permit local network access.

## Tree of Thoughts

Tree of Thoughts prompting is designed to "[generalize] over the popular Chain of Thought approach" by enabling the exploration and node-wise evaluation over a tree of potential reasoning paths. The code repo associated with the paper is hosted by princeton-nlp [here](https://github.com/princeton-nlp/tree-of-thought-llm).

ToT, as implemented and described in the paper, uses BFS with pruning to traverse the tree, where pruning decisions are arrived at through a [reflexion-style](https://arxiv.org/abs/2303.11366) process, wherein the model itself evaluated path quality. My implementation only currently uses DFS, and the quantized model is not very reliable for path evaluation, so I intend to replace path scoring with head-to-head comparisons ("voting" in the original paper/repo) to remedy this and then implement BFS. In a BFS scenario, a tree depth of N yields (numLeaves)^N nodes (before pruning) to propose/evaluate in parallel, which makes it a great application for multithreading, especially since each proposal/evaluation takes a variable and unknown amount of time to return. I use a semaphore alongside an atomic queue to manage access to compute resources (servers), abstracting the details into a `serverQueue` class that combines semaphore waits and releases with dequeue and enqueue operations, respectively, to ensure queue state is always as expected. Each process, upon receiving server access, initiates a stream from the server's Ollama instance, which is disposed of upon completion alongside server/semaphore release.

Right now, there is limited interactivity in the actual API, as most of the values are currently hardcoded, rather that user-provided in POST requests. This is primarily for development purposes, so that I can use GET requests with my examples and defaults more effortlessly. I use a Postgresql database to manage proposal and evaluation prompts, but there is no user model or auth, because the project is designed for personal, home-network use. Ultimately, the idea would be to have a CLI on each of my machines that checks all networked devices for open ollama default ports (11434) and then issues my LLM queries as ToT queries, which are parallelized by this program across all of my machines. q4_0 quantized 7B models can run surprisingly quickly on Macbooks, in addition to GPU-equipped PCs, so it's not crazy to expect a solid performance boost by using every machine you can get your hands on at once. 

## Citations

@misc{yao2023tree,
      title={{Tree of Thoughts}: Deliberate Problem Solving with Large Language Models}, 
      author={Shunyu Yao and Dian Yu and Jeffrey Zhao and Izhak Shafran and Thomas L. Griffiths and Yuan Cao and Karthik Narasimhan},
      year={2023},
      eprint={2305.10601},
      archivePrefix={arXiv},
      primaryClass={cs.CL}
}
