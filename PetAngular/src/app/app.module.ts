import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { TokenService } from './APIGetters/token.service';
import { ErrorHandlerProvider } from './APIGetters/error.handler';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { AppComponent } from './app.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MainComponent } from './main/main.component';
import { MenuComponent } from './menu/menu.component';
import { RegistrationComponent } from './registration/registration.component';
import { AlertifyService } from './APIGetters/alertify.service';
import { PetsComponent } from './pets/pets.component';
import { LikesComponent } from './likes/likes.component';
import { RouterModule } from '@angular/router';
import { routes } from './routing';
import { AuthorizationGuard } from './protectionService/authorization.guard';
import { PetService } from './APIGetters/pet.service';
import { JwtModule } from '@auth0/angular-jwt';
import { SinglepetComponent } from './singlepet/singlepet.component';
import { PeteditionComponent } from './petedition/petedition.component';
import { FileUploadModule } from 'ng2-file-upload';

export function GetToken() {
   return localStorage.getItem('token');
 }

@NgModule({
   declarations: [
      AppComponent,
      MenuComponent,
      MainComponent,
      RegistrationComponent,
      PetsComponent,
      LikesComponent,
      SinglepetComponent,
      PeteditionComponent
   ],
   imports: [
      BrowserModule,
      FormsModule,
      HttpClientModule,
      RouterModule.forRoot(routes, {
         onSameUrlNavigation: 'reload',
       }),
      JwtModule.forRoot({
         config: {
           tokenGetter: GetToken,
           whitelistedDomains: ['localhost:5000'],
           blacklistedRoutes: ['localhost:5000/api/authorization']
         }
       }),
       TabsModule.forRoot(),
       ReactiveFormsModule,
       NgxGalleryModule,
       FileUploadModule

   ],
   providers: [
      TokenService,
      ErrorHandlerProvider,
      AlertifyService,
      AuthorizationGuard,
      PetService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
