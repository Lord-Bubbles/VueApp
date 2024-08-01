import { defineStore } from 'pinia';
import { ref } from 'vue';
import router from '@/router/index';

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null);
  const token = ref(null);

  async function login(form) {
    const response = await fetch('/api/Auth/authenticate', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json'
      },
      body: JSON.stringify(form)
    });
    if (!response.ok) {
      throw new Error(response.statusText);
    }
    const data = await response.json();
    user.value = data.user;
    token.value = data.accessToken;
    await router.replace({ name: 'home' });
  }

  async function logout() {
    await fetch('/api/Auth/logout', {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token.value}`
      }
    });
    token.value = null;
    user.value = null;
    await router.replace({ name: 'login' });
  }

  async function refresh() {
    const response = await fetch('/api/Auth/refresh', {
      method: 'POST'
    });
    if (!response.ok) {
      console.log('Error: invalid refresh token!');
      user.value = null;
      token.value = null;
      await router.replace({ name: 'login' });
      throw new Error();
    }
    const data = await response.json();
    token.value = data.accessToken;
    user.value = data.user;
  }

  async function register(form) {
    const response = await fetch('/api/Auth/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json'
      },
      body: JSON.stringify(form)
    });
    if (!response.ok) {
      throw new Error(response.statusText);
    }
    await router.replace({ name: 'login' });
  }

  return { user, token, login, logout, refresh, register };
});
