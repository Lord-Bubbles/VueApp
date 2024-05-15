import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap';
import 'bootstrap-icons/font/bootstrap-icons.min.css';
import './assets/main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
//import { VueQueryPlugin } from 'vue-query';
import App from './App.vue';
import router from './router';

const pinia = createPinia();
const app = createApp(App);

app.use(router);
app.use(pinia);
//app.use(VueQueryPlugin);

app.mount('#app');
