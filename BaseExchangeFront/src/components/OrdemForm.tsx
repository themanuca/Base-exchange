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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: name === 'quantidade' || name === 'preco' ? Number(value) : value,
    }));
  };

  const enviar = async (e: React.FormEvent) => {
    e.preventDefault();
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
      setResposta({ sucesso: false, exposicao_atual: 0, msg_erro: 'Erro ao enviar ordem.' });
    }
  };

  return (
    <form onSubmit={enviar} className="max-w-md mx-auto p-4 bg-white shadow-md rounded space-y-4">
      <h2 className="text-xl font-semibold">Enviar Ordem</h2>

      <select name="ativo" value={form.ativo} onChange={handleChange} className="w-full border p-2 rounded">
        <option value="PETR4">PETR4</option>
        <option value="VALE3">VALE3</option>
        <option value="VIIA4">VIIA4</option>
      </select>

      <select name="lado" value={form.lado} onChange={handleChange} className="w-full border p-2 rounded">
        <option value="Compra">Compra</option>
        <option value="Venda">Venda</option>
      </select>

      <input type="number" name="quantidade" placeholder="Quantidade"
        className="w-full border p-2 rounded" min={1} max={99999}
        value={form.quantidade} onChange={handleChange} />

      <input type="number" name="preco" placeholder="Preço"
        className="w-full border p-2 rounded" step="0.01" max={999.99}
        value={form.preco} onChange={handleChange} />

      <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
        Enviar Ordem
      </button>

      {resposta && (
        <div className={`p-2 rounded ${resposta.sucesso ? 'bg-green-100' : 'bg-red-100'}`}>
          <p><strong>Exposição Atual:</strong> R$ {resposta.exposicao_atual.toFixed(2)}</p>
          {!resposta.sucesso && <p className="text-red-500">{resposta.msg_erro}</p>}
        </div>
      )}
    </form>
  );
}
