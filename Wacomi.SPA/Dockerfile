# base image
FROM node:6.10 as node

WORKDIR /usr/src/app

COPY package.json package.json
RUN npm install --silent
RUN npm install -g @angular/cli@1.6.8

COPY . .
#RUN ng build --prod --aot
RUN ng build --aot

FROM nginx 
COPY ./nginx-custom.conf /etc/nginx/conf.d/default.conf
COPY --from=node /usr/src/app/dist /usr/share/nginx/html
EXPOSE 4200

# start app
# CMD ng serve --host 0.0.0.0