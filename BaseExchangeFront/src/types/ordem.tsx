export type Ativo = 'PETR4' | 'VALE3' | 'VIIA4';
export type Lado = 'Compra' | 'Venda';

export interface OrdemDTO {
  ativo: Ativo;
  lado: Lado;
  quantidade: number;
  preco: number;
}

export interface RespostaOrdem {
  Sucesso: boolean;
  ExposicaoAtual: number;
  MsgErro?: string;
}
