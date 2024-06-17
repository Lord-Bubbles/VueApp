import { defineStore } from 'pinia';
import { ref } from 'vue';
import router from '@/router/index';

export const useAuthStore = defineStore('auth', () => {
  const user = ref(JSON.parse(localStorage.getItem('user')));

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
      alert('Incorrect username or password!');
      return;
    }
    const data = await response.json();
    user.value = data.user;
    localStorage.setItem('token', data.accessToken);
    localStorage.setItem('user', JSON.stringify(data.user));
    await router.push({ name: 'home', params: { id: data.user.id } });
  }

  async function logout() {
    await fetch('/api/Auth/logout', {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`
      }
    });
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    user.value = null;
    await router.push({ name: 'login' });
  }

  async function refresh() {
    const response = await fetch('/api/Auth/refresh', {
      method: 'POST'
    });
    if (!response.ok) {
      console.log('Error: invalid refresh token!');
      user.value = null;
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      await router.push({ name: 'login' });
      return;
    }
    const data = await response.json();
    localStorage.setItem('token', data.accessToken);
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
      alert('This user already exists. Please login instead!');
      return;
    }
    await router.push({ name: 'login' });
  }

  return { user, login, logout, refresh, register };
});
