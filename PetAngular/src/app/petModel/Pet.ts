import { Photo } from './Photo';

export interface Pet {
    id: number;
    name: string;
    gender: string;
    city: string;
    age: number;
    photo: string;
    description?: string;
    photos?: Photo[];
}
