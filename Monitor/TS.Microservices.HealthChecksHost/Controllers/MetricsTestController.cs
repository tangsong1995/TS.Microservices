using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.Threading.Tasks;
using Metric = Prometheus.Metrics;

namespace TS.Microservices.HealthChecksHost.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MetricsTestController : ControllerBase
    {
        [HttpGet]
        public async Task GetMetricsAsync()
        {
            var r = Metric.NewCustomRegistry();
            MetricFactory f = Metric.WithCustomRegistry(r);
            r.AddBeforeCollectCallback(() =>
            {
                f.CreateCounter("counter_test", "").Inc(100);
            });
            Response.ContentType = PrometheusConstants.ExporterContentType;
            Response.StatusCode = 200;
            await r.CollectAndExportAsTextAsync(Response.Body, HttpContext.RequestAborted);
        }
    }
}
