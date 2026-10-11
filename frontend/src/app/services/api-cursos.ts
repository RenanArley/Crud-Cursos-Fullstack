import { HttpClient } from '@angular/common/http';
import { inject, Injectable} from '@angular/core';
import { Observable } from 'rxjs';
import { Curso } from '../Models/curso';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ApiCursos {
    protected readonly baseUrl: string = `${environment.apiUrl}/cursos/`;
    protected readonly HttpClient: HttpClient = inject(HttpClient);

    ReadAll(): Observable<Curso[]>{
        return this.HttpClient.get<Curso[]>(this.baseUrl);        
    }

    ReadById(id: number): Observable<Curso>{
        return this.HttpClient.get<Curso>(`${this.baseUrl}${id}`);
    }

    Create(curso: Curso): Observable<Curso>{
        return this.HttpClient.post<Curso>(this.baseUrl, curso);
    }
    
        Update(curso: Curso): Observable<Curso> {
        return this.HttpClient.put<Curso>(`${this.baseUrl}${curso.id}`, curso);
    }

    Delete(id: number): Observable<void> {
        return this.HttpClient.delete<void>(`${this.baseUrl}${id}`);
    }

}
