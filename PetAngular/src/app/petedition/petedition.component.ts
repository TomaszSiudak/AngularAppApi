import { Component, OnInit } from '@angular/core';
import { TokenService } from '../APIGetters/token.service';
import { Pet } from '../petModel/Pet';
import { Photo } from '../petModel/Photo';
import { FileUploader } from 'ng2-file-upload';
import { ActivatedRoute } from '@angular/router';
import { PetService } from '../APIGetters/pet.service';
import { AlertifyService } from '../APIGetters/alertify.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-petedition',
  templateUrl: './petedition.component.html',
  styleUrls: ['./petedition.component.css']
})
export class PeteditionComponent implements OnInit {
  pet: Pet;
  photos: Photo[];
  uploader: FileUploader;
  url = environment.api;
  //currentMain: Photo;

  constructor(private tokenService: TokenService, private petService: PetService,
     private route: ActivatedRoute, private alertify: AlertifyService) { }


  ngOnInit() {
    this.getPet();
    this.setup();
  }

  setup()
   {
    this.uploader = new FileUploader({
      url: this.url + 'pets/' + this.tokenService.getUserIdFromToken() + '/photo',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false
    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const responsePhoto: Photo = JSON.parse(response);
        const photo = {
          id: responsePhoto.id,
          url: responsePhoto.url,
          description: responsePhoto.description,
          mainPhoto: responsePhoto.mainPhoto
        };
        this.photos.push(photo);
        this.alertify.ok('Zdjęcie zostało dodane pomyślnie.');
      }
    };
  }
  getPet()
  {
    this.petService.getPet(this.route.snapshot.params['petId']).subscribe((pet: Pet) => {
      console.log('testujmy ' + pet);
      this.pet = pet;
      this.photos = pet.photos;
    }, error => {
      console.log(error);
    });

  }

  update()
  {
    this.petService.update(this.pet, +this.route.snapshot.params['petId']).subscribe(next => {
      console.log(this.pet);
      this.alertify.ok('Zmiany zapisane');
    }, error => {
      this.alertify.error(error);
    });
  }

  setPhoto(photo: Photo)
   {
    this.petService.setPhoto(this.tokenService.getUserIdFromToken(), photo.id).subscribe(() => {
      this.alertify.ok('Zdjęcie główne zostało zmienione. Zmiany będą widoczne na profilu');
    }, error => {
      this.alertify.error(error);
    });
  }

  deletePhoto(photo: Photo)
  {
   this.petService.deletePhoto(this.tokenService.getUserIdFromToken(), photo.id).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photo.id), 1);
      this.alertify.ok('Zdjęcie zostało usunięte.');
   }, error => {
     this.alertify.error(error);
   });
 }

}
