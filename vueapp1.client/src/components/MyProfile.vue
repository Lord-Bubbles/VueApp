<script setup>
  import { ref, computed, inject } from 'vue';
  const { user, updateUser } = inject('user');

  const formData = ref({
    id: user?.id,
    firstName: user?.firstName,
    lastName: user?.lastName,
    email: user?.email,
    age: user?.age,
    phoneNum: user?.phoneNum,
    birthday: user?.birthday,
    managerName: user?.managerName,
    account: user?.account
  });

  const formPass = ref({
    password: '',
    checkPassword: ''
  });

  const saveData = async () => {
    try {
      const response = await fetch(`/api/User/${formData.value.id}`, {
        method: 'PUT',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData.value)
      });
      await response.json();
      updateUser(formData.value);
    } catch (error) {
      console.log('An error has occurred while updating user data: ' + error);
    }
    if (formPass.value.password && formPass.value.password === formPass.value.checkPassword) {
      try {
        const response = await fetch(`/api/User/${formData.value.id}/password`, {
          method: 'PUT',
          headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(formPass.value.password)
        });
        await response.json();
      } catch (error) {
        console.log('An error has occured while updating password: ' + error);
      }
    }
  };

  const formattedBirthday = computed({
    get() {
      const date = formData.value.birthday ? new Date(formData.value.birthday) : new Date();
      const dateTimeFormat = new Intl.DateTimeFormat('en-CA');
      const string = dateTimeFormat.format(date);
      return string;
    },
    set(newValue) {
      formData.value.birthday = new Date(newValue).toISOString();
    }
  });
</script>

<template>
  <div>
    <form @submit.prevent="saveData">
      <div class="row mb-3">
        <div class="col">
          <label class="form-label">First Name</label>
          <input class="form-control" v-model="formData.firstName" />
        </div>
        <div class="col">
          <label class="form-label">Last Name</label>
          <input class="form-control" v-model="formData.lastName" />
        </div>
      </div>
      <div class="mb-3">
        <label class="form-label">Email</label>
        <input class="form-control" disabled v-model="formData.email" />
      </div>
      <div class="row mb-3">
        <div class="col">
          <label class="form-label">Age</label>
          <input type="number" class="form-control" v-model="formData.age" />
        </div>
        <div class="col">
          <label class="form-label">Phone Number</label>
          <input type="tel" class="form-control" v-model="formData.phoneNum" />
        </div>
      </div>
      <div class="row mb-3">
        <div class="col">
          <label class="form-label">Birthday</label>
          <input type="date" class="form-control" v-model="formattedBirthday" />
        </div>
        <div class="col">
          <label class="form-label">Manager</label>
          <input type="text" class="form-control" v-model="formData.managerName" disabled />
        </div>
      </div>
      <div class="mb-3">
        <label class="form-label">Change Password</label>
        <input
          type="password"
          placeholder="New password"
          autocomplete="new-password"
          class="form-control mb-3"
          v-model="formPass.password"
        />
        <input
          type="password"
          placeholder="Confirm new password"
          autocomplete="new-password"
          class="form-control"
          v-model="formPass.checkPassword"
        />
      </div>
      <button type="submit" class="btn btn-primary">Save Changes</button>
    </form>
  </div>
</template>