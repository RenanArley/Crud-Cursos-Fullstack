import { Component, inject, signal, WritableSignal,  } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { ApiCursos } from '../../../../services/api-cursos';
import { Curso } from '../../../../Models/curso';

@Component({
  imports: [FormsModule],
  selector: 'app-cursos-editar',
  styleUrl: './cursos-editar.css',
  templateUrl: './cursos-editar.html',
})
export class CursosEditar {
  private readonly router: Router = inject(Router);
  private readonly apiCursos: ApiCursos = inject(ApiCursos);
  private readonly route: ActivatedRoute = inject(ActivatedRoute);

  protected readonly curso: WritableSignal<Curso> = signal<Curso>({} as Curso);
  private id: number = 0;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.apiCursos.ReadById(this.id).subscribe({
      next: (curso: Curso) => {
        this.curso.set(curso);
      },
      error: (error) => {
        alert('Erro ao carregar o curso.');
        console.error('Erro ao carregar o curso', error);
      }
    });
  }

  RedirectToListar(): void {
    this.router.navigate(['/cursos/listar']);
  }

  Update(): void {
    this.apiCursos.Update(this.curso()).subscribe({
      next: () =>{
        alert('Curso atualizado com sucesso!');
        this.RedirectToListar();
      },
      error: (error) => {
        alert('Erro ao atualizar o curso.');
        console.error('Erro ao atualizar o curso', error);
      }
    });
  }
}
