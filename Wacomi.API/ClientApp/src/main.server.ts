import 'zone.js/dist/zone-node';
import 'reflect-metadata';
import { renderModule, renderModuleFactory } from '@angular/platform-server';
import { APP_BASE_HREF } from '@angular/common';
import { enableProdMode } from '@angular/core';
import { provideModuleMap } from '@nguniversal/module-map-ngfactory-loader';
import { createServerRenderer } from 'aspnet-prerendering';
export { AppServerModule } from './app/app.server.module';

enableProdMode();

export default createServerRenderer(params => {
//   return new Promise(function (resolve, reject) {
//     var result = '<h1>Hello world!</h1>'
//         + '<p>Current time in Node is: ' + new Date() + '</p>'
//         + '<p>Request is: ' + params.data.Request + '</p>'
//         + '<p>Request path is: ' + params.location.path + '</p>'
//         + '<p>Absolute URL is: ' + params.absoluteUrl + '</p>'
//         + '<p>URL is: ' + params.url + '</p>'
//         + '<p>Origin is: ' + params.origin + '</p>'
//         + '<p>Base URL is: ' + params.baseUrl + '</p>'
//         + '<p>Original Html: ' + params.data.originalHtml + '</p>';

//     resolve({ html: result });
// });

  const { AppServerModule, AppServerModuleNgFactory, LAZY_MODULE_MAP } = (module as any).exports;
 
  console.log(params);

  const options = {
    document: params.data.originalHtml,
    url: params.url,
    extraProviders: [
      provideModuleMap(LAZY_MODULE_MAP),
      { provide: APP_BASE_HREF, useValue: params.baseUrl },
      { provide: 'BASE_URL', useValue: params.origin + params.baseUrl }
    ]
  };

  console.log(options);

  const renderPromise = renderModuleFactory(AppServerModuleNgFactory, options);
    
  return renderPromise.then(html => ({ html }));
});