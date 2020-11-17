import { Injectable } from '@angular/core';
import { Pet } from '../petModel/Pet';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Pages } from '../petModel/Pagination';
import { map } from 'rxjs/operators';



@Injectable({
  providedIn: 'root'
})
export class PetService {
  url = environment.api;

  constructor(private client: HttpClient) {}

  getPets(page?, pageSize?): Observable<Pages<Pet[]>>
  {
    const pages: Pages<Pet[]> = new Pages<Pet[]>();
    let params = new HttpParams();

    if(page != null && pageSize != null)
    { 
      params = params.append('currentPage', page);
      params = params.append('pageSize', pageSize);
    }

    /*return this.client.get<Pet[]>(this.url + 'pets', {observe: 'response', params}).
    pipe(map(response =>
      {
         pages.result = response.body;
         if(response.headers.get('Pagination') != null)
      }));*/

      // tslint:disable-next-line: align
      return this.client.get<Pet[]>(this.url + 'pets', { observe: 'response', params})
      .pipe(
        map(response => {
          pages.result = response.body;
          if (response.headers.get('Pagination') != null) {
            pages.pagination = JSON.parse(response.headers.get('Pagination'))
          }
          return pages;
        })
      );
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
