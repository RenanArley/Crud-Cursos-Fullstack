import { Component, inject, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ApiCursos } from '../../../../services/api-cursos';
import { Curso } from '../../../../Models/curso';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  imports: [RouterLink, FormsModule, CommonModule],
  selector: 'app-cursos-listar',
  styleUrl: './cursos-listar.css',
  templateUrl: './cursos-listar.html',
})
export class CursosListar {
  protected readonly apiCursos: ApiCursos = inject(ApiCursos);
  protected readonly router: Router = inject(Router);
  protected searchId: string = '';

  protected cursos:  WritableSignal<Curso[]> = signal<Curso[]>([]);

  Get(): void{
    if (this.searchId !== null && this.searchId !== ''){
      this.apiCursos.ReadById(Number(this.searchId)).subscribe({
        next: (curso: Curso)=>{
        this.cursos.set([curso])
        }, error: (error: HttpErrorResponse)=>{
          if (error.status === 404){
            alert("Nenhum curso encontrado com esse Id");
            this.cursos.set([]);
          }
        }
      })
      return;
    }
    this.apiCursos.ReadAll().subscribe({
      next: (cursos: Curso[])=>{
        this.cursos.set(cursos);        
      }, error: (error: HttpErrorResponse) =>{
        alert("Não foi possível carregar a lista de cursos.");
        console.error('erro ao buscar curso', error);
      }
    })
  }

  RedirectToCadastrar(): void{
    this.router.navigate(['/cursos/cadastrar'])
  }

    Delete(id: number): void {

    if (confirm('Deseja realmente excluir este curso?')) {
      this.apiCursos.Delete(id).subscribe({
        next: () => {
          alert('Curso excluído com sucesso!.');
          this.Get();
        },

        error: (error: HttpErrorResponse) => {
          if (error.status === 409) {
            alert('Não é possível excluir um curso ativo.');
          } else {
            alert('Erro ao excluir o curso. ');
          }        
        }
      });      
    }
  }
}
