export interface Curso {
    id?: number;
    nome: string;
    cargaHoraria: number; 
    valor: number| string;
    dataInicio: string;
    online: boolean;
    ativo: boolean;

}
