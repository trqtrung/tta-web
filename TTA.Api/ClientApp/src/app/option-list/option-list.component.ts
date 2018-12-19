import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import {OptionList} from './shared/option-list.model';
import {ActivatedRoute} from '@angular/router';
import {MatPaginator, MatSort, MatTableDataSource} from '@angular/material';
import {CollectionViewer, DataSource} from '@angular/cdk/collections';
import {Observable} from 'rxjs/Observable';
import {OptionListService} from './shared/option-list.service';
import {debounceTime, distinctUntilChanged, startWith, tap, delay} from 'rxjs/operators';
import {merge} from 'rxjs/observable/merge';
import {fromEvent} from 'rxjs/observable/fromEvent';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {catchError, finalize} from 'rxjs/operators';
import {of} from 'rxjs/observable/of';
import { Operator } from 'rxjs';

@Component({
  selector: 'app-option-list',
  templateUrl: './option-list.component.html',
  styleUrls: ['./option-list.component.css']
})
export class OptionListComponent implements OnInit, AfterViewInit {

  dataSource: OptionsDataSource;

  displayedColumns = ['id', 'name', 'key', 'value'];

  @ViewChild(MatPaginator) paginator: MatPaginator;

    @ViewChild(MatSort) sort: MatSort;

    @ViewChild('input') input: ElementRef;

    constructor(private route: ActivatedRoute,
                private optionListService: OptionListService) {

    }

  ngOnInit() {
    this.dataSource = new OptionsDataSource(this.optionListService);
    this.dataSource.loadOptions('', 0, 25);
  }

  ngAfterViewInit() {

    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    fromEvent(this.input.nativeElement, 'keyup')
        .pipe(
            debounceTime(150),
            distinctUntilChanged(),
            tap(() => {
                this.paginator.pageIndex = 0;

                this.loadPage();
            })
        )
        .subscribe();

    merge(this.sort.sortChange, this.paginator.page)
    .pipe(
        tap(() => this.loadPage())
    )
    .subscribe();

  }

  loadPage() {
      this.dataSource.loadOptions(
          this.input.nativeElement.value,
          this.paginator.pageIndex,
          this.paginator.pageSize);
  }
}

export class OptionsDataSource extends DataSource<OptionList> {

  private optionsSubject = new BehaviorSubject<OptionList[]>([]);

  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();

  constructor(private optionListService: OptionListService) {
    super();
  }

  loadOptions(filter: string,
              pageIndex: number,
              pageSize: number) {

        this.loadingSubject.next(true);

        this.optionListService.searchOptionList(filter, pageIndex, pageSize).pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((options) => {
                this.optionsSubject.next(options);
            console.log(options);
            });
        }

  connect(collectionViewer: CollectionViewer): Observable<OptionList[]> {
      console.log('Connecting data source');
      return this.optionsSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
      this.optionsSubject.complete();
      this.loadingSubject.complete();
  }

}
