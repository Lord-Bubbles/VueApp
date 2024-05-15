import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import MyProfile from '../components/MyProfile.vue';
import PerformancePage from '../components/PerformancePage.vue';
import LoginView from '../views/LoginView.vue';
import SignupView from '../views/SignupView.vue';
import AdminPage from '../components/AdminPage.vue';
import ManagerPage from '../components/ManagerPage.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/user/:id',
      name: 'home',
      component: HomeView,
      children: [
        {
          path: 'profile',
          name: 'profile',
          component: MyProfile
        },
        {
          path: 'performance',
          name: 'performance',
          component: PerformancePage
        },
        {
          path: 'admin',
          name: 'admin',
          component: AdminPage
        },
        {
          path: 'manager',
          name: 'manager',
          component: ManagerPage
        }
      ]
    },
    {
      path: '/',
      children: [
        {
          path: '',
          name: 'login',
          component: LoginView,
          alias: '/login'
        },
        { path: '/signup', name: 'signup', component: SignupView }
      ]
    }
  ]
});

export default router;
