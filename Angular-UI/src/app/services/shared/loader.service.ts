import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class LoaderService {

  constructor(private toastr: ToastrService) { }

  showSuccess(msg: string) {
    this.toastr.success(msg);
  }

  showWarning(msg: string) {
    this.toastr.warning(msg);
  }

  showError(msg: string) {
    this.toastr.error(msg);
  }

  showLoading(show: boolean) {
    var loaderObject = document.getElementById('loading-div');
    if (loaderObject != undefined && loaderObject != null)
      if (show)
        document.getElementById('loading-div').style.display = 'block';
      else
        document.getElementById('loading-div').style.display = 'none';
  }
}