import { Component, OnInit } from '@angular/core';
import { Pet } from '../petModel/Pet';
import { PetService } from '../APIGetters/pet.service';
import { TokenService } from '../APIGetters/token.service';
import { AlertifyService } from '../APIGetters/alertify.service';
import { Pages, Pagination } from '../petModel/Pagination';

@Component({
  selector: 'app-pets',
  templateUrl: './pets.component.html',
  styleUrls: ['./pets.component.css']
})
export class PetsComponent implements OnInit {
  pets: Pet[];
  likerId: any = {};
  currentPage = 1;
  pageSize = 8;
  pagination: Pagination;
  genders = [{value: 'female', display: 'Female'}, {value: 'male', display: 'Male'} ];
  types = [{value: 'dog', display: 'Pies'}, {value: 'cat', display: 'Kot'},
   {value: 'rabbit', display: 'Królik'}, {value: 'hamster', display: 'Chomik'}, {value: 'parrot', display: 'Papuga'} ];
   petParams: any = {};


  constructor(private petService: PetService, private tokenService: TokenService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getPets();
    this.petParams.gender = null;
    this.petParams.type = null;
  }

 getPets()
 {
   this.petService.getPets(this.currentPage, this.pageSize, this.petParams.gender, this.petParams.type).subscribe((pets: Pages<Pet[]>) =>
    {
      this.pets = pets.result;
      this.pagination = pets.pagination;
      console.log(this.pagination);
    }, error => {
      console.log(error);
    });
 }

 getPets2(page?, pageSize?)
 {
   this.petService.getPets(page, pageSize, this.petParams.gender, this.petParams.type).subscribe((pets: Pages<Pet[]>) =>
    {
      this.pets = pets.result;
      this.pagination = pets.pagination;
      console.log(this.pagination);
    }, error => {
      console.log(error);
    });
 }

 resetFilters() {
  this.petParams.gender = null;
  this.petParams.type = null;
  this.getPets();
}

 pageChanged(event: any): void {
  this.pagination.currentPage = event.page;
  this.getPets2(this.pagination.currentPage, this.pagination.pageSize);
}

 addLike(petId: number, name: any )
 {
   this.petService.AddLike(petId, this.tokenService.getUserIdFromToken()).subscribe(data =>
    {
      this.alertify.ok('Polubiłeś użytkownika ' + name);
    }, error => {
      this.alertify.info('Użytkownik został już polubiony');
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
