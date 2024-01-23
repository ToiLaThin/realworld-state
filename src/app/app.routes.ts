import { Routes } from '@angular/router';

export const appRoutes: Routes = [
    {
        path: '',
        //promise lazy loading
        loadChildren: () => import('./home/home.module').then(m => m.HomeModule)
    }
];
