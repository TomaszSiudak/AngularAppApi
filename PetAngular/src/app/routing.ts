import {Routes} from '@angular/router';
import { MainComponent } from './main/main.component';
import { PetsComponent } from './pets/pets.component';
import { LikesComponent } from './likes/likes.component';
import { AuthorizationGuard } from './protectionService/authorization.guard';
import { SinglepetComponent } from './singlepet/singlepet.component';
import { PeteditionComponent } from './petedition/petedition.component';

export const routes: Routes = [
    {path: 'main', component: MainComponent},
    {path: 'pets', component: PetsComponent, canActivate: [AuthorizationGuard]},
    {path: 'pets/edition/:petId', component: PeteditionComponent, canActivate: [AuthorizationGuard]},
    {path: 'likes', component: LikesComponent, canActivate: [AuthorizationGuard]},
    {path: 'pets/:petId', component: SinglepetComponent, canActivate: [AuthorizationGuard]},
    {path: '**', redirectTo: 'main', pathMatch: 'full'}
];
