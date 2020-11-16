import { Component, OnInit } from '@angular/core';
import { Pet } from '../petModel/Pet';
import { PetService } from '../APIGetters/pet.service';
import { TokenService } from '../APIGetters/token.service';
import { AlertifyService } from '../APIGetters/alertify.service';

@Component({
  selector: 'app-pets',
  templateUrl: './pets.component.html',
  styleUrls: ['./pets.component.css']
})
export class PetsComponent implements OnInit {
  pets: Pet[];
  likerId: any = {};


  constructor(private petService: PetService, private tokenService: TokenService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getPets();
  }

 getPets()
 {
   this.petService.getPets().subscribe((pets: Pet[]) =>
    {
      this.pets = pets;
    }, error => {
      console.log(error);
    });
 }

 addLike(petId: number, name: any )
 {
   this.petService.AddLike(petId, this.tokenService.getUserIdFromToken()).subscribe(data =>
    {
      this.alertify.ok('Polubiłeś użytkownika ' + name);
    }, error => {
      this.alertify.info("Użytkownik został już polubiony");
    });
 }

 isProfileOfCurrentUser(petId: number)
 {
   const idFromToken = this.tokenService.getUserIdFromToken();
   if (petId == idFromToken)
   {
     return true;
   } else
   {
     return false;
   }

 }


}
