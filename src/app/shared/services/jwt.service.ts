import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
    //life cycle of the service
})
export class JwtService {
    jwtKey = 'jwtToken';
    getToken(): string | null {
        return window.localStorage[this.jwtKey];
    }

    saveToken(token: string) {
        window.localStorage[this.jwtKey] = token;
    }

    destroyToken() {
        window.localStorage.removeItem(this.jwtKey);
    }
}