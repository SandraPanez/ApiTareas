using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ApiTareas.ML;

namespace ApiTareas.Controllers;

[ApiController]
[Route("api/ml")]
public class MlController : ControllerBase
{
    private static MLContext _mlContext = new MLContext();
    private static ITransformer? _modelo;

    private static ITransformer GetModelo()
    {
        if (_modelo != null) return _modelo;

        var dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "sentimientos.csv");
        var data = _mlContext.Data.LoadFromTextFile<SentimentData>(dataPath, hasHeader: true, separatorChar: ',');

        var pipeline = _mlContext.Transforms.Text
            .FeaturizeText("Features", nameof(SentimentData.Comentario))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        _modelo = pipeline.Fit(data);
        return _modelo;
    }

    [HttpPost("sentimiento")]
    public IActionResult AnalizarSentimiento([FromBody] SentimentRequest request)
    {
        var modelo = GetModelo();
        var engine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(modelo);

        var prediccion = engine.Predict(new SentimentData { Comentario = request.Comentario });

        return Ok(new
        {
            comentario = request.Comentario,
            sentimiento = prediccion.Prediccion ? "Positivo" : "Negativo"
        });
    }
}

public class SentimentRequest
{
    public string Comentario { get; set; } = string.Empty;
}