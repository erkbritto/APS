using Microsoft.AspNetCore.Mvc;
using APS.Models;
using APS.Data;

namespace APS.Controllers
{
    public class EmergyController : Controller
    {
        private readonly DatabaseService _dbService;

        public EmergyController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalcularEstatico(double mj)
        {
            if (mj < 0)
            {
                ModelState.AddModelError("mj", "O valor em MJ deve ser maior ou igual a zero.");
                return View("Index");
            }

            double resultado = mj * 1.96e4;
            var calculo = new Calculo
            {
                Tipo = "estatico",
                Entrada = mj,
                Resultado = resultado,
                Timestamp = DateTime.Now
            };

            try
            {
                _dbService.SaveCalculo(calculo);
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = $"Erro ao salvar no banco de dados: {ex.Message}";
                return View("Index");
            }

            string mensagem = resultado > 3.56e7
                ? $"Alerta: Emergência alta! {resultado} Msej"
                : $"Emergia: {resultado} Msej";

            ViewBag.Mensagem = mensagem;
            return View("Index");
        }

        [HttpPost]
        public IActionResult CalcularDinamico(double estoque)
        {
            if (estoque < 0)
            {
                ModelState.AddModelError("estoque", "O estoque inicial deve ser maior ou igual a zero.");
                return View("Index");
            }

            double resultado = estoque * 0.9;
            var calculo = new Calculo
            {
                Tipo = "dinamico",
                Entrada = estoque,
                Resultado = resultado,
                Timestamp = DateTime.Now
            };

            try
            {
                _dbService.SaveCalculo(calculo);
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = $"Erro ao salvar no banco de dados: {ex.Message}";
                return View("Index");
            }

            string mensagem = $"Estoque após 1h: {resultado} kg";
            ViewBag.Mensagem = mensagem;
            return View("Index");
        }
    }
}