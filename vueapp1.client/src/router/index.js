import { useAuthStore } from '@/stores/authStore';
import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/user/:id',
      name: 'home',
      component: () => import('@/pages/HomePage.vue'),
      children: [
        {
          path: 'profile',
          name: 'profile',
          component: () => import('@/pages/ProfilePage.vue')
        },
        {
          path: 'performance',
          name: 'performance',
          component: () => import('@/pages/PerformancePage.vue')
        },
        {
          path: 'admin',
          name: 'admin',
          component: () => import('@/pages/AdminPage.vue')
        },
        {
          path: 'manager',
          name: 'manager',
          component: () => import('@/pages/ManagerPage.vue')
        }
      ],
      meta: { requiresAuth: true }
    },
    {
      path: '/',
      children: [
        {
          path: '',
          name: 'login',
          component: () => import('@/pages/LoginPage.vue'),
          alias: '/login'
        },
        { path: '/signup', name: 'signup', component: () => import('@/pages/RegisterPage.vue') }
      ],
      meta: { requiresAuth: false }
    }
  ]
});

// Route guard to redirect to login page if user isn't authenticated
router.beforeEach(async (to) => {
  const authStore = useAuthStore();

  if (to.meta.requiresAuth) {
    if (!authStore.token) {
      // upon refresh, only get new access/refresh token pair if token state is not valid
      try {
        await authStore.refresh();
      } catch {
        // Invalid refresh token so redirect to login page
        return {
          name: 'login'
        };
      }
    }
  }
});

export default router;
