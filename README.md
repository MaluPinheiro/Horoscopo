# 🔮 CavaleirosDoZodiaco – API de Horóscopo

Este projeto é uma API ASP.NET Core que fornece previsões de horóscopo **diárias**, **semanais** e **mensais**, consumindo dados de uma API externa. Usuários podem se cadastrar com plano **básico** ou **avançado**, onde o plano avançado oferece informações extras.

---

## 🧩 Estrutura do Projeto

```
CavaleirosDoZodiaco/
│
├── Controllers/
│   └── HoroscopoController.cs          # Endpoints da API
│
├── Models/
│   ├── HoroscopoDailyApiResponse.cs   # Modelo para resposta diária da API externa
│   ├── HoroscopoWeeklyApiResponse.cs  # Modelo para resposta semanal da API externa
│   ├── HoroscopoMonthlyApiResponse.cs # Modelo para resposta mensal da API externa
│   ├── HoroscopoResponse.cs           # Modelo de resposta final enviada ao cliente
│   └── Usuario.cs                     # Modelo de cadastro de usuários
│
├── Services/
│   └── HoroscopoService.cs            # Lógica para chamadas
│
├── Cliente/
│   └── cliente.py                     # Script em Python que consome a API via terminal
│
├── Usuarios.json                      # Armazena usuários cadastrados (plano e nickname)
├── appsettings.json                   # Configurações do projeto
├── Program.cs                         # Configuração e inicialização da aplicação
```

---

## ✅ Pré-requisitos

- [.NET 6 SDK ou superior](https://dotnet.microsoft.com/en-us/download)
- [Python 3.10 ou superior](https://www.python.org/downloads/)
- Visual Studio (ou VS Code com extensão C#)

---

## 🚀 Como executar

### 1. Clonar o projeto

```bash
git clone https://github.com/seu-usuario/CavaleirosDoZodiaco.git
cd CavaleirosDoZodiaco
```

### 2. Execute o código no Visual Studio 2022

- Aperte **F5** para executar
- Será executado na porta `localhost:5082` (ou conforme configurado)

### 3. Rodar o cliente Python

- Abra o terminal na pasta `Cliente/`
- Execute:
```bash
python cliente.py
```

## 📄 Exemplo de resultado

**Plano Avançado**:

```json
{
  "signo": "Taurus",
  "mensagem": "...",
  "data": "May 25, 2025",
  "numeroDaSorte": "26",
  "corDoDia": "Azul",
  "bichoDoDia": "Gato"
}
```

**Plano Básico**:

```json
{
  "signo": "Taurus",
  "mensagem": "...",
  "data": "May 25, 2025"
}
```

---

## ⚠️ Observações

- Caso veja `Failed to fetch`, verifique:
  - Se o navegador está tentando usar `https` em vez de `http`
  - Se a porta local está correta
  - Se há bloqueio por CORS

- O `Usuarios.json` será criado após o primeiro cadastro.
- O cliente Python usa o módulo `requests`. Se não estiver instalado, rode:
```bash
pip install requests
```
