import { Component, inject } from '@angular/core';
import { ApiCursos } from '../../../../services/api-cursos';
import { Router } from '@angular/router';
import { Curso } from '../../../../Models/curso';
import { FormsModule } from '@angular/forms';

@Component({
  imports: [FormsModule],
  selector: 'app-cursos-cadastrar',
  styleUrl: './cursos-cadastrar.css',
  templateUrl: './cursos-cadastrar.html',
})
export class CursosCadastrar {
  protected readonly apiCursos: ApiCursos = inject(ApiCursos);
  protected readonly router: Router = inject(Router);
  protected readonly dataMinima: string = new Date().toISOString().substring(0, 10);
  protected readonly curso: Curso = {
    nome: '',
    cargaHoraria: 0,
    valor: 0,
    dataInicio: '',
    online: false,
    ativo: true    
  }

  Create(): void {
    this.apiCursos.Create(this.curso).subscribe({
      next: (curso: Curso) => {
        alert('Curso cadastrado com sucesso!');
        this.RedirectToListar();
      },
      error: (error) => {
        alert('Erro ao cadastrar o curso.');
        console.error('Erro ao cadastrar o curso', error);
      }
    });
  }

  RedirectToListar(): void {
    this.router.navigate(['/cursos/listar']);
  }
}
