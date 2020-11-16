import { Injectable } from '@angular/core';
declare let alertify: any;
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

areYouSure(message: string, callback: () => any) {
  alertify.confirm(message, function(e) {
    if (e) {
      callback();
    } else {}
  });
}

ok(message: string) {
  alertify.success(message);
}

error(message: string) {
  alertify.error(message);
}

warning(message: string) {
  alertify.warning(message);
}

info(message: string) {
  alertify.message(message);
}
}
