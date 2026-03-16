using APS.Models;
using System.Collections.Generic;

namespace APS.Data
{
    public interface IDatabaseService
    {
        Task SaveCalculoAsync(Calculo calculo);
        Task<List<Calculo>> GetAllCalculosAsync();
        Task DeleteCalculoAsync(int id);
    }

    public class DatabaseService : IDatabaseService
    {
        // Simulando um "banco de dados" em memória para evitar dependências de pacotes
        private static readonly List<Calculo> _calculos = new();
        private static int _nextId = 1;

        public DatabaseService()
        {
        }

        public async Task SaveCalculoAsync(Calculo calculo)
        {
            if (calculo == null)
                throw new ArgumentNullException(nameof(calculo));

            try
            {
                calculo.Id = _nextId++;
                _calculos.Add(calculo);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Erro ao salvar cálculo: {ex}");
                throw;
            }
        }

        public async Task<List<Calculo>> GetAllCalculosAsync()
        {
            try
            {
                // Retorna em ordem decrescente por timestamp
                return await Task.FromResult(
                    _calculos.OrderByDescending(c => c.Timestamp).ToList()
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Erro ao recuperar cálculos: {ex}");
                throw;
            }
        }

        public async Task DeleteCalculoAsync(int id)
        {
            try
            {
                var calculo = _calculos.FirstOrDefault(c => c.Id == id);
                if (calculo != null)
                {
                    _calculos.Remove(calculo);
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Erro ao deletar cálculo: {ex}");
                throw;
            }
        }
    }
}

