using System.Text.Json.Serialization;

namespace OllamaTot.Dtos
{
  public class OllamaRequest
  {
    public OllamaRequest(OllamaRequest request)
    {
      Model = request.Model;
      Prompt = request.Prompt;
      Format = request.Format;
      Options = request.Options;
      System = request.System;
      Template = request.Template;
      Context = request.Context;
      Stream = request.Stream;
      Raw = request.Raw;
    }

    public OllamaRequest(string model)
    {
      Model = model;
      Raw = true;
      Options = new OllamaRequestOptions();
    }

    #pragma warning disable CS8618
    [JsonPropertyName("model")]
    public string Model { get; set; }
    #pragma warning restore CS8618

    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }

    [JsonPropertyName("format")]
    public string? Format { get; set; }

    [JsonPropertyName("options")]
    public OllamaRequestOptions Options { get; set; }

    [JsonPropertyName("system")]
    public string? System { get; set; }

    [JsonPropertyName("template")]
    public string? Template { get; set; }

    [JsonPropertyName("context")]
    public int[]? Context { get; set; }

    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    [JsonPropertyName("raw")]
    public bool? Raw { get; set; }
  }

  public class OllamaRequestOptions
  {
    public OllamaRequestOptions()
    {
    }

    [JsonPropertyName("num_keep")]
    public int? NumKeep { get; set; }

    [JsonPropertyName("seed")]
    public int? Seed { get; set; }

    [JsonPropertyName("num_predict")]
    public int? NumPredict { get; set; }

    [JsonPropertyName("top_k")]
    public int? TopK { get; set; }

    [JsonPropertyName("top_p")]
    public double? TopP { get; set; }

    [JsonPropertyName("tfs_z")]
    public double? TfsZ { get; set; }

    [JsonPropertyName("typical_p")]
    public double? TypicalP { get; set; }

    [JsonPropertyName("repeat_last_n")]
    public int? RepeatLastN { get; set; }

    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    [JsonPropertyName("repeat_penalty")]
    public double? RepeatPenalty { get; set; }

    [JsonPropertyName("presence_penalty")]
    public double? PresencePenalty { get; set; }

    [JsonPropertyName("frequency_penalty")]
    public double? FrequencyPenalty { get; set; }

    [JsonPropertyName("mirostat")]
    public int? Mirostat { get; set; }

    [JsonPropertyName("mirostat_tau")]
    public double? MirostatTau { get; set; }

    [JsonPropertyName("mirostat_eta")]
    public double? MirostatEta { get; set; }

    [JsonPropertyName("penalize_newline")]
    public bool? PenalizeNewline { get; set; }

    [JsonPropertyName("stop")]
    public string[]? Stop { get; set; }

    [JsonPropertyName("numa")]
    public bool? Numa { get; set; }

    [JsonPropertyName("num_ctx")]
    public int? NumCtx { get; set; }

    [JsonPropertyName("num_batch")]
    public int? NumBatch { get; set; }

    [JsonPropertyName("num_gqa")]
    public int? NumGqa { get; set; }

    [JsonPropertyName("num_gpu")]
    public int? NumGpu { get; set; }

    [JsonPropertyName("main_gpu")]
    public int? MainGpu { get; set; }

    [JsonPropertyName("low_vram")]
    public bool? LowVram { get; set; }

    [JsonPropertyName("f16_kv")]
    public bool? F16Kv { get; set; }

    [JsonPropertyName("logits_all")]
    public bool? LogitsAll { get; set; }

    [JsonPropertyName("vocab_only")]
    public bool? VocabOnly { get; set; }

    [JsonPropertyName("use_mmap")]
    public bool? UseMmap { get; set; }

    [JsonPropertyName("use_mlock")]
    public bool? UseMlock { get; set; }

    [JsonPropertyName("embedding_only")]
    public bool? EmbeddingOnly { get; set; }

    [JsonPropertyName("rope_frequency_base")]
    public double? RopeFrequencyBase { get; set; }

    [JsonPropertyName("rope_frequency_scale")]
    public double? RopeFrequencyScale { get; set; }

    [JsonPropertyName("num_thread")]
    public int? NumThread { get; set; }
  }

  public class OllamaResponse
  {
    #pragma warning disable CS8618
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("response")]
    public string Response { get; set; }
    #pragma warning restore CS8618

    [JsonPropertyName("context")]
    public int[]? Context { get; set; }

    [JsonPropertyName("done")]
    public bool? Done { get; set; }

    [JsonPropertyName("total_duration")]
    public long? TotalDuration { get; set; }

    [JsonPropertyName("load_duration")]
    public long? LoadDuration { get; set; }

    [JsonPropertyName("sample_count")]
    public int? SampleCount { get; set; }

    [JsonPropertyName("sample_duration")]
    public long? SampleDuration { get; set; }

    [JsonPropertyName("prompt_eval_count")]
    public int? PromptEvalCount { get; set; }

    [JsonPropertyName("prompt_eval_duration")]
    public long? PromptEvalDuration { get; set; }

    [JsonPropertyName("eval_count")]
    public int? EvalCount { get; set; }

    [JsonPropertyName("eval_duration")]
    public long? EvalDuration { get; set; }
  }
}
