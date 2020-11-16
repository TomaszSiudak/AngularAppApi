import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { TokenService } from '../APIGetters/token.service';
import { AlertifyService } from '../APIGetters/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuard implements CanActivate {
  constructor(private tokenService: TokenService, private router: Router, private alertify: AlertifyService) {}

  canActivate(): boolean {
    if (this.tokenService.logged()) {return true;}
    this.alertify.error('Aby uzyskać dostęp do zdjęć zarejestruj/zaloguj się');
    this.router.navigate(['/main']);
    return false;
  }
}
