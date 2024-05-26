import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useAuthStore = defineStore('authStore', () => {
  const userProfile = ref(JSON.parse(localStorage.getItem('user')));

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
