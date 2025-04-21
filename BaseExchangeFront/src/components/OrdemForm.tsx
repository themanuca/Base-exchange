import { useState } from 'react';
import { OrdemDTO, RespostaOrdem } from '../types/ordem';
import { api } from '../services/api';

const initialState: OrdemDTO = {
  ativo: 'PETR4',
  lado: 'Compra',
  quantidade: 0,
  preco: 0,
};

export function OrderForm() {
  const [form, setForm] = useState(initialState);
  const [resposta, setResposta] = useState<RespostaOrdem | null>(null);
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: name === 'quantidade' || name === 'preco' ? Number(value) : value,
    }));
  };

  const validar = () => {
    const err: Record<string, string> = {};
    if (!form.ativo){
        err.ativo = 'Ativo é obrigatório';
    } 
    if (!form.lado){
        err.lado = 'Lado é obrigatório';
    }
    if (form.quantidade <= 0){
        err.quantidade = 'Quantidade deve ser maior que zero';
    }
    if (form.preco <= 0){
        err.preco = 'Preço deve ser maior que zero';
    }
    setErrors(err);
    return Object.keys(err).length === 0;
  };

  const enviar = async (e: React.FormEvent) => {
    debugger
    e.preventDefault();
    if (!validar()){
        return;
    }

    setLoading(true);
    setResposta(null);

    try {
      const payload = {
        ativo: form.ativo,
        lado: form.lado === 'Compra' ? 'C' : 'V',
        quantidade: form.quantidade,
        preco: form.preco,
      };
      const { data } = await api.post<RespostaOrdem>('/ordem', payload);
      setResposta(data);
    } catch (err: any) {
      setResposta({
        sucesso: false,
        exposicao_atual: 0,
        msg_erro: err.response?.data?.message || 'Erro ao enviar ordem.',
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={enviar} className="max-w-md mx-auto p-4 bg-white shadow-md rounded space-y-4">
      <h2 className="text-xl font-semibold">Enviar Ordem</h2>

      <div>
        <label>Ativo:</label>
        <select name="ativo" value={form.ativo} onChange={handleChange} className="w-full border p-2 rounded">
          <option value="">Selecione</option>
          <option value="PETR4">PETR4</option>
          <option value="VALE3">VALE3</option>
          <option value="VIIA4">VIIA4</option>
        </select>
        {errors.ativo && <p className="text-sm text-red-500">{errors.ativo}</p>}
      </div>

      <div>
        <label>Ordem:</label>
        <select name="lado" value={form.lado} onChange={handleChange} className="w-full border p-2 rounded">
          <option value="Compra">Compra</option>
          <option value="Venda">Venda</option>
        </select>
        {errors.lado && <p className="text-sm text-red-500">{errors.lado}</p>}
      </div>

      <div>
        <label>Quantidade:</label>
        <input
          type="number"
          name="quantidade"
          className="w-full border p-2 rounded"
          min={1}
          value={form.quantidade}
          onChange={handleChange}
        />
        {errors.quantidade && <p className="text-sm text-red-500">{errors.quantidade}</p>}
      </div>

      <div>
        <label>Preço:</label>
        <input
          type="number"
          name="preco"
          className="w-full border p-2 rounded"
          step="0.01"
          value={form.preco}
          onChange={handleChange}
        />
        {errors.preco && <p className="text-sm text-red-500">{errors.preco}</p>}
      </div>

      <button
        type="submit"
        disabled={loading}
        className={`w-full py-2 px-4 rounded text-white ${loading ? 'bg-gray-400' : 'bg-blue-500 hover:bg-blue-600'}`}
      >
        {loading ? 'Enviando...' : 'Enviar Ordem'}
      </button>

      {resposta && (
        <div className={`p-2 mt-4 rounded ${resposta.sucesso ? 'bg-green-100' : 'bg-red-100'}`}>
          <p>
            <strong>Exposição Atual:</strong> R$ {resposta.exposicao_atual.toFixed(2)}
          </p>
          {!resposta.sucesso && <p className="text-red-500">{resposta.msg_erro}</p>}
        </div>
      )}
    </form>
  );
}
