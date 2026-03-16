using Microsoft.AspNetCore.Mvc;
using APS.Models;
using APS.Data;
using System.ComponentModel.DataAnnotations;

namespace APS.Controllers
{
    public class EmergyController : Controller
    {
        private readonly IDatabaseService _dbService;
        private const double FATOR_MJ_PARA_MSEJ = 1.96e4;
        private const double LIMIAR_ALERTA = 3.56e7;

        public EmergyController(IDatabaseService dbService)
        {
            _dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var calculos = await _dbService.GetAllCalculosAsync();
                return View(calculos);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao carregar cálculos: {ex.Message}";
                return View(new List<Calculo>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalcularEstatico([Range(0, double.MaxValue, ErrorMessage = "O valor deve ser maior ou igual a zero")] double mj)
        {
            if (!ModelState.IsValid)
            {
                return await RetornarIndex("O valor em MJ deve ser válido e maior ou igual a zero.");
            }

            try
            {
                double resultado = mj * FATOR_MJ_PARA_MSEJ;
                var calculo = new Calculo
                {
                    Tipo = "estático",
                    Entrada = mj,
                    Resultado = resultado,
                    Timestamp = DateTime.Now
                };

                await _dbService.SaveCalculoAsync(calculo);

                string mensagem = resultado > LIMIAR_ALERTA
                    ? $"⚠️ ALERTA: Emergência alta! {resultado:F2} Msej"
                    : $"✓ Emérgia: {resultado:F2} Msej";

                ViewBag.Mensagem = mensagem;
                ViewBag.TipoMensagem = resultado > LIMIAR_ALERTA ? "alert-danger" : "alert-success";
                return await RetornarIndex(null);
            }
            catch (Exception ex)
            {
                return await RetornarIndex($"Erro ao salvar no banco de dados: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalcularDinamico([Range(0, double.MaxValue, ErrorMessage = "O estoque deve ser maior ou igual a zero")] double estoque)
        {
            if (!ModelState.IsValid)
            {
                return await RetornarIndex("O estoque inicial deve ser válido e maior ou igual a zero.");
            }

            try
            {
                double resultado = estoque * 0.9;
                var calculo = new Calculo
                {
                    Tipo = "dinâmico",
                    Entrada = estoque,
                    Resultado = resultado,
                    Timestamp = DateTime.Now
                };

                await _dbService.SaveCalculoAsync(calculo);

                string mensagem = $"✓ Estoque após 1h: {resultado:F2} kg";
                ViewBag.Mensagem = mensagem;
                ViewBag.TipoMensagem = "alert-info";
                return await RetornarIndex(null);
            }
            catch (Exception ex)
            {
                return await RetornarIndex($"Erro ao salvar no banco de dados: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                await _dbService.DeleteCalculoAsync(id);
                ViewBag.Mensagem = "✓ Cálculo deletado com sucesso!";
                ViewBag.TipoMensagem = "alert-success";
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = $"Erro ao deletar: {ex.Message}";
                ViewBag.TipoMensagem = "alert-danger";
            }

            return await RetornarIndex(null);
        }

        private async Task<IActionResult> RetornarIndex(string? mensagemErro)
        {
            try
            {
                var calculos = await _dbService.GetAllCalculosAsync();
                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    ViewBag.Erro = mensagemErro;
                    ViewBag.TipoMensagem = "alert-danger";
                }
                return View("Index", calculos);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao carregar a página: {ex.Message}";
                return View("Index", new List<Calculo>());
            }
        }
    }
}
