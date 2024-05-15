<script setup>
  import { ref, inject, watchEffect } from 'vue';
  import ReviewFormModal from './ReviewFormModal.vue';
  // import { orderBy } from 'lodash';

  const users = ref([]);
  const { user } = inject('user');
  const manager = user?.firstName + ' ' + user?.lastName;
  const modal = ref(false);

  watchEffect(async () => {
    try {
      const response = await fetch(`/api/User?managerName=${manager}&page=1&limit=50`);
      const data = await response.json();
      users.value = data;
    } catch (error) {
      console.log('Error has occurred while fetching... ' + error);
    }
  });

  const fields = ['firstName', 'lastName', 'age', 'email', 'birthday', 'phoneNum'];
  const headers = ['First Name', 'Last Name', 'Age', 'Email', 'Birthday', 'Phone Number'];

  const displayData = (user, field) => {
    if (field === 'birthday') {
      return new Date(user[field]).toLocaleDateString('en-US', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
      });
    }
    return user[field];
  };
</script>

<template>
  <main class="container m-4 p-4">
    <h1 class="mb-3">My Team Members</h1>
    <div v-if="users.length > 0" class="table-responsive">
      <table class="table table-striped table-hover table-bordered table-dark">
        <thead>
          <tr>
            <th v-for="field in headers" :key="'team-' + field">
              {{ field }}
            </th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user of users" :key="user.id">
            <td v-for="field in fields" :key="field">
              {{ displayData(user, field) }}
            </td>
            <td>
              <button class="btn btn-primary" type="button" @click="() => (modal = true)">
                Add Review
              </button>
            </td>
            <ReviewFormModal v-if="modal" v-model="modal" type="manager" :userID="user.id" />
          </tr>
        </tbody>
      </table>
    </div>
  </main>
</template>