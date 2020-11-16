import { Injectable } from '@angular/core';
import { Pet } from '../petModel/Pet';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PetService {
  url = environment.api;

  constructor(private client: HttpClient) {}

  getPets(): Observable<Pet[]>
  {
    return this.client.get<Pet[]>(this.url + 'pets');
  }

  getPet(petId): Observable<Pet>
  {
    return this.client.get<Pet>(this.url + 'pets/' + petId);
  }

  update(pet: Pet, petId: number)
  {
    return this.client.put(this.url + 'pets/' + petId, pet);
  }

  setPhoto(petId: number, photoId: number)
  {
    return this.client.post(this.url + 'pets/' + petId + '/photo/' + photoId + '/setMainPhoto', '');
  }

  deletePhoto(petId: number, photoId: number)
  {
    return this.client.delete(this.url + 'pets/' + petId + '/photo/' + photoId);
  }

  AddLike(petId: number, likerId: number)
  {
    return this.client.post(this.url + 'pets/' + petId + '/likes/' + likerId, '');
  }

  getPetsWhichLiked(petId: number): Observable<Pet[]>
  {
    return this.client.get<Pet[]>(this.url + 'pets/' + petId + '/likes');
  }
}
