import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  registration = false;

  constructor() { }

  ngOnInit() {
  }

  goToRegistration(){
    this.registration = true;
  }

  cancelRegistration(isRegistrationFormVisible: boolean)
  {
    this.registration = isRegistrationFormVisible;

  }

}
