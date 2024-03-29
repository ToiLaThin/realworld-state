import { Routes } from '@angular/router';

export const appRoutes: Routes = [
    {
        path: '',
        //promise lazy loading
        loadChildren: () => import('./home/home.module').then(m => m.HomeModule),        
    },
    {
        path: 'settings',
        loadChildren: () => import('./settings/settings.module').then(m => m.SettingModule),
    },
    {
        path: 'editor',
        loadChildren: () => import('./editor/editor.module').then(m => m.EditorModule),
    },
    {
        path: 'profile',
        loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule),
    },
    {
        path: 'article',
        loadChildren: () => import('./article/article.module').then(m => m.ArticleModule),
    }
];
