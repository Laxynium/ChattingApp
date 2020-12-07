import {Pipe, PipeTransform} from '@angular/core';
@Pipe({name: 'timespan'})
export class TimepsanPipe implements PipeTransform {
  transform(value: string): string {
    const splitted = value.split(':');
    if (splitted.length == 0) return '-';
    if (splitted.length == 1) return `${splitted[0]}h`;
    if (splitted.length >= 2) return `${splitted[0]}h ${splitted[1]}m`;
  }
}
