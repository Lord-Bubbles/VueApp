<script setup>
  import { computed, ref, inject } from 'vue';

  const props = defineProps(['mode', 'data']);
  const modal = defineModel();
  const emit = defineEmits(['onUpdate']);

  const { managerNames, getManagers } = inject('allManagers');
  const managerInput = ref(null);

  const formData = ref({
    id: props.data?.id,
    firstName: props.data?.firstName,
    lastName: props.data?.lastName,
    email: props.data?.email,
    age: props.data?.age,
    birthday: props.data?.birthday,
    phoneNum: props.data?.phoneNum,
    account: { type: props.data?.account.type },
    managerName: props.data?.managerName
  });

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

  const filteredManagers = computed(() => {
    const filter = managerNames.filter((n) =>
      n.toLowerCase().includes(formData.value.managerName.toLowerCase())
    );
    return filter.length > 0 ? filter : ['No results'];
  });

  const updateData = async () => {
    try {
      await fetch(`/api/User/${formData.value.id}`, {
        method: props.mode === 'edit' ? 'PUT' : 'DELETE',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json'
        },
        body: props.mode === 'edit' ? JSON.stringify(formData.value) : null
      });
      modal.value = false;
      emit('onUpdate');
      await getManagers();
    } catch (error) {
      console.log('An error has occurred while updating! ' + error);
    }
  };
</script>

<template>
  <div
    class="modal fade"
    data-bs-backdrop="static"
    tabindex="-1"
    data-bs-keyboard="false"
    :class="{ show: modal, 'd-block': modal }"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content text-light-emphasis">
        <div class="modal-header">
          <button
            type="button"
            class="btn-close"
            @click="() => (modal = false)"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="updateData">
            <span v-if="props.mode === 'delete'">Are you sure you want to delete this user?</span>
            <div v-else>
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
                <input class="form-control" v-model="formData.email" />
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
                  <label class="form-label">Employee Type</label>
                  <select class="form-select" v-model="formData.account.type">
                    <option>admin</option>
                    <option>manager</option>
                    <option>employee</option>
                  </select>
                </div>
              </div>
              <div class="mb-3">
                <label class="form-label">Manager</label>
                <div class="dropdown">
                  <input
                    ref="managerInput"
                    type="text"
                    class="form-control"
                    v-model="formData.managerName"
                    placeholder="Manager Name"
                    data-bs-toggle="dropdown"
                  />
                  <ul class="dropdown-menu">
                    <li v-for="(name, index) in filteredManagers" :key="name + '-' + index">
                      <button
                        type="button"
                        class="dropdown-item"
                        :disabled="name === 'No results' ? true : false"
                        @click="() => (managerInput.value = name)"
                      >
                        {{ name }}
                      </button>
                    </li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="d-flex justify-content-evenly mt-3">
              <button
                type="submit"
                class="btn"
                :class="{
                  'btn-danger': props.mode === 'delete',
                  'btn-primary': props.mode === 'edit'
                }"
              >
                {{ props.mode === 'delete' ? 'Yes' : 'Save Changes' }}
              </button>
              <button type="button" class="btn btn-secondary" @click="() => (modal = false)">
                Cancel
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>