
# BaseExchangeFront

Frontend desenvolvido em **React + TypeScript + Vite** para consumir a API do projeto [Base-exchange](https://github.com/themanuca/Base-exchange).

---

## 🚀 Tecnologias Utilizadas

- React 18
- TypeScript
- Vite
- Axios
- Tailwind CSS
- Docker

---

## 🎯 Funcionalidade

A aplicação permite:

- Enviar ordens de compra ou venda de ativos
- Exibir a exposição financeira atual após cada operação
- Validar os campos do formulário
- Exibir mensagens de erro ou sucesso
- Listar todas as ordens já registradas

---
## 🐳 Executando tudo com Docker
A aplicação completa (API + Banco + Frontend) pode ser executada via Docker com:

``` 
docker-compose up --build
```
---

## 📦 Instalação

```bash
git clone https://github.com/themanuca/Base-exchange.git
cd Base-exchange/BaseExchangeFront
npm install
```

---

## ▶️ Execução

```bash
npm run dev
```

A aplicação será aberta em `http://localhost:5173` por padrão.

---

## 🔌 Configuração de API

A API está configurada para rodar por padrão em `http://localhost:8080/api`.  
Certifique-se de que a [API Backend](https://github.com/themanuca/Base-exchange) está rodando antes de utilizar o frontend.

Você pode alterar a base da URL no arquivo `src/services/api.ts`:

```ts
export const api = axios.create({
  baseURL: 'http://localhost:8080/api',
});
```

---

## 💅 Estilo

O projeto utiliza **Tailwind CSS** para estilos rápidos e responsivos.

---

## 🧪 Validação

Todos os campos do formulário são validados antes de enviar a requisição:

- `ativo` é obrigatório
- `lado` é obrigatório (compra ou venda)
- `quantidade` deve ser maior que zero
- `preço` deve ser maior que zero

---

