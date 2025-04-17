# 💻 APS - Arquitetura de Redes

## 📌 Proposta do Projeto

Este projeto é uma **Atividade Prática Supervisionada (APS)** da disciplina de **Arquitetura de Redes**.  
O objetivo é desenvolver uma **infraestrutura de rede distribuída** para cálculos emergéticos, inspirada no software **SCALE** (Marvuglia et al., 2013).

A proposta envolve uma **LAN com três nós** — dois clientes e um servidor — que utilizam **C# (ASP.NET Core)** e **MySQL** para processar emergia estática e dinâmica.  
A rede é simulada no **Cisco Packet Tracer**, permitindo análises de tráfego e desempenho.

---

## 🌍 Contexto e Inspiração

### ⚡ Emergia
- Conceito de **H.T. Odum (1998)**: energia total (em **joules solares - sej**) usada para gerar um recurso ou serviço.  
- Exemplo: `1 MJ = 1.96E+04 Msej`.

### 🧠 SCALE
- Ferramenta (Marvuglia et al., 2013) que calcula emergia a partir de **inventários de ciclo de vida (LCI)**.
- Usa algoritmos de **rastreamento** e **busca em grafos**.

### 🔧 Adaptação no Projeto
- Cálculos emergéticos **estáticos simplificados**.
- Função **dinâmica** inspirada no EmSim, simulando **variações temporais** (ex: queda de estoque ao longo do tempo).

---

## 🎯 Objetivos

- Configurar uma **LAN** com comunicação via **HTTP** entre clientes e servidor.  
- Desenvolver software em **C# (ASP.NET Core)** + **MySQL** para cálculos emergéticos.  
- Simular a rede no **Cisco Packet Tracer** e analisar seu desempenho.  
- Produzir **documentação completa**: relatório, simulação, slides e banner.

---

## 🧱 Estrutura do Projeto

### 1. Infraestrutura de Rede

- **Topologia:** LAN com switch **Cisco 2950** e três nós:
  - **Cliente 1 (192.168.1.2):** envia dados em MJ/estoques.
  - **Cliente 2 (192.168.1.3):** complementa os dados.
  - **Servidor (192.168.1.4):** processa os cálculos.
- **Protocolo:** HTTP  
- **Segurança:** Autenticação básica no servidor  
- **Simulação:** Cisco Packet Tracer com tráfego leve (1 KB) e pesado (1 MB)

### 2. Software

- **Linguagem:** C# (ASP.NET Core)  
- **Banco de Dados:** MySQL (`tabela: calculos`)  

#### Funcionalidades:
- **Cálculo Estático:**  
  `MJ → Msej` (Ex: `1 MJ = 1.96E+04 Msej`)  
- **Cálculo Dinâmico:**  
  Estoque reduzido em **10% por hora**  
- **Alertas:**  
  Gera alerta quando emergia ultrapassa **3.56E+07 Msej**

#### Exemplo de Código:
```csharp
[HttpPost]
public IActionResult CalcularEstatico(double mj)
{
    double resultado = mj * 1.96e4;
    // Armazena no MySQL e retorna resultado
    return Ok(resultado);
}
```

### 3. Documentação

- **Relatório Técnico:** Fonte Arial 12, espaçamento 1,5  
- **Simulação:** Arquivo `.pkt` (Cisco Packet Tracer)  
- **Apresentação:** Slides (`.pptx`) + Banner (`.jpg`)  
- **Fichas APS:** Consolidado em `.pdf`

---

## 👥 Organização do Grupo

| Função                | Responsável                      |
|-----------------------|----------------------------------|
| Líder                 | Documentação e apresentação      |
| Especialista em Redes | Simulação no Packet Tracer       |
| Desenvolvedor         | Backend em C# + banco de dados   |

### 🗓️ Cronograma (10 semanas)

| Semana | Atividade                         |
|--------|-----------------------------------|
| 1      | Definição do grupo e escopo       |
| 2-4    | Planejamento: rede, software, teoria |
| 5-7    | Execução: LAN, código, testes     |
| 8-9    | Testes finais                     |
| 10     | Finalização e entrega            |

---

## 🧪 Testes Realizados

| Teste        | Entrada           | Resultado Esperado                    |
|--------------|-------------------|---------------------------------------|
| Teste 1      | `mj = 1`          | Emergia: `1.96E+04 Msej`              |
| Teste 2      | `estoque = 100kg` | Após 1h: `90 kg`                      |
| Teste 3      | `mj = 2000`       | Alerta: "Emergia alta! 3.92E+07 Msej" |

---

## ✅ Resultados Esperados

- LAN funcional (**latência < 5 ms**)  
- Cálculos emergéticos **precisos** armazenados no MySQL  
- Documentação clara, simulação validada

---

## 📚 Referências

- Marvuglia, A., Benetto, E., Rios, G., & Rugani, B. (2013).  
  *SCALE: Software for CALculating Emergy based on life cycle inventories*.  
  Ecological Modelling, 248, 80-91.

- Odum, H.T. (1998).  
  *Advances in Energy Studies: Energy Flows in Ecology and Economy*.  
  International Workshop, Italy.
