using Microsoft.ML.Data;

namespace ApiTareas.ML;

public class SentimentData
{
    [LoadColumn(0)]
    public string Comentario { get; set; } = string.Empty;

    [LoadColumn(1)]
    [ColumnName("Label")]
    public bool EsPositivo { get; set; }
}

public class SentimentPrediction : SentimentData
{
    [ColumnName("PredictedLabel")]
    public bool Prediccion { get; set; }
}