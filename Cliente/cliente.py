import requests

def cadastrar_usuario(base_url, nickname, plano):
    url = f"{base_url}/cadastro"
    payload = {
        "nickname": nickname,
        "plano": plano
    }

    try:
        response = requests.post(url, json=payload)
        response.raise_for_status()
        print(f"Cadastro realizado: {response.text}")
    except requests.exceptions.RequestException as e:
        print(f"Erro ao cadastrar usuário: {e}")
        exit()

def obter_horoscopo():
    base_url = "http://localhost:5082/api/horoscope"

    nickname = input("Digite seu nickname: ").strip()
    plano = input("Digite seu plano (basico ou avancado): ").strip().lower()

    cadastrar_usuario(base_url, nickname, plano)

    signo = input("Digite seu signo (ex: Taurus, Aries): ").strip()
    tipo = input("Tipo de horóscopo (diario, semanal, mensal): ").strip().lower()

    params = {
        "signo": signo,
        "nickname": nickname
    }

    if tipo == "diario":
        dia = input("Digite o dia (TODAY, TOMORROW, YESTERDAY ou YYYY-MM-DD): ").strip()
        params["dia"] = dia
        endpoint = f"{base_url}/diario"
    elif tipo == "semanal":
        endpoint = f"{base_url}/semanal"
    elif tipo == "mensal":
        endpoint = f"{base_url}/mensal"
    else:
        print("Tipo inválido.")
        return

    try:
        response = requests.get(endpoint, params=params)
        response.raise_for_status()
        dados = response.json()

        print("\nResultado:")
        print(f"Signo: {dados.get('signo')}")
        print(f"Data: {dados.get('data')}")
        print(f"Mensagem: {dados.get('mensagem')}")

        if "numeroDaSorte" in dados:
            print(f"Número da sorte: {dados['numeroDaSorte']}")
        if "corDoDia" in dados:
            print(f"Cor do dia: {dados['corDoDia']}")
        if "bichoDoDia" in dados:
            print(f"Bicho do dia: {dados['bichoDoDia']}")

    except requests.exceptions.RequestException as e:
        print(f"Erro ao conectar com a API: {e}")
    except ValueError:
        print("Erro ao processar resposta JSON.")

if __name__ == "__main__":
    obter_horoscopo()
