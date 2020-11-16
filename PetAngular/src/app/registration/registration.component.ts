import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TokenService } from '../APIGetters/token.service';
import { AlertifyService } from '../APIGetters/alertify.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  petData: any = {};
  @Output() cancel = new EventEmitter();
  regForm: FormGroup;

  constructor(private authorizationAPI: TokenService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.regForm = new FormGroup({
     name: new FormControl('', [Validators.required]),
     password: new FormControl('', [Validators.required, Validators.minLength(4)]),
     confirmPassword: new FormControl('', [Validators.required]),
     gender: new FormControl('male'),
     type: new FormControl('', [Validators.required]),
     city: new FormControl()
    }, this.passwordValidator);
  }

  passwordValidator(g: FormGroup)
  {
    return g.get('password').value === g.get('confirmPassword').value ? null : {'x': true};
  }

  register()
  {
    if (this.regForm.valid)
    {
      this.petData = Object.assign({}, this.regForm.value);
      this.authorizationAPI.register(this.petData).subscribe(() => {
        this.alertify.ok('Rejestracja przebiegła pomyślnie. Można się zalogować');
      }, error => {
        if(error == 'Server Error')
        {
          this.alertify.error("Użytkownik o tej nazwie już istnieje. Wybierz inną.");
        } else
        {
          this.alertify.error(error);
        }
      }, () => {
          this.cancelRegistration();
      });
    }
  }

  cancelRegistration()
  {
    this.cancel.emit(false);
    console.log('cancel');
  }

}
