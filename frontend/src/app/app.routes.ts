import { Routes } from '@angular/router';
import { Home } from './components/pages/home/home';
import { CursosListar } from './components/pages/cursos/cursos-listar/cursos-listar';
import { CursosEditar } from './components/pages/cursos/cursos-editar/cursos-editar';
import { pattern } from '@angular/forms/signals';
import { CursosCadastrar } from './components/pages/cursos/cursos-cadastrar/cursos-cadastrar';
import { Error404 } from './components/pages/error-404/error-404';


export const routes: Routes = [
    {path:'', redirectTo: 'home', pathMatch: 'full'},
    {path: 'home', component: Home},
    {path: 'cursos/listar', component: CursosListar},
    {path: 'cursos/cadastrar', component: CursosCadastrar},
    {path: 'cursos/editar/:id', component: CursosEditar},
    {path: '**', component: Error404}

];
