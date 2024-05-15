import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useAuthStore = defineStore('authStore', () => {
  const userProfile = ref(
    localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')) : null
  );

  function updateUser(user) {
    userProfile.value = user;
    localStorage.setItem('user', JSON.stringify(user));
  }

  function removeUser() {
    userProfile.value = null;
    localStorage.removeItem('user');
  }

  return { userProfile, updateUser, removeUser };
});
