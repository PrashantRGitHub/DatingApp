import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login()
  {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged In Successfull');
        // const user = next;
        // if (user){
        //   localStorage.setItem('token', user.token);
        //   this.alertify.success(next.message);
        // }
    }, error => {
      // console.log(error);
      this.alertify.error(error);
    });
  }

  loggedIn()
  {
    return this.authService.loggedin();
  }

  loggedOut()
  {
    localStorage.removeItem('token');
    // console.log('Logged Out');
    this.alertify.message('Logged Out');
  }

}
