import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {Observable} from 'rxjs/Observable';
import {OptionListService} from "./option-list.service";
import { OptionList } from "./option-list.model";



@Injectable()
export class OptionListResolver implements Resolve<OptionList[]> {

    constructor(private optionListService: OptionListService) {

    }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<OptionList[]> {
        return this.optionListService.searchOptionList();
    }
}