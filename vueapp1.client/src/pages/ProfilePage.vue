<script setup>
  import { useAuthStore } from '@/stores/authStore';
  import { updateUser } from '@/utils/userService';
  import { storeToRefs } from 'pinia';
  import { date, object, ref, string } from 'yup';
  import { Form, Field, ErrorMessage } from 'vee-validate';
  import { useToast } from 'vue-toastification';

  const authStore = useAuthStore();
  const { user } = storeToRefs(authStore);

  const schema = object().shape({
    firstName: string().required('First name is required'),
    lastName: string().required('Last name is required'),
    birthday: date().max(new Date(), "Birthday can't be a future date"),
    phoneNum: string().matches(/^\d{3}-\d{3}-\d{4}$/, {
      message: 'Phone number must be in the form: xxx-xxx-xxxx',
      excludeEmptyString: true
    }),
    password: string().matches(/^[\w\W]{8,}$/, {
      message: 'Password must be at least 8 characters long',
      excludeEmptyString: true
    }),
    confirmPassword: string().oneOf([ref('password')], "Passwords don't match")
  });

  const initialValues = {
    firstName: user.value.firstName,
    lastName: user.value.lastName,
    phoneNum: user.value.phoneNum,
    birthday: user.value.birthday,
    password: '',
    confirmPassword: ''
  };

  const saveData = async (values) => {
    const toast = useToast();
    try {
      await updateUser(authStore.user.id, values);
    } catch (error) {
      if (error.message != 'Bad Request') {
        toast.error('An unexpected error occurred while updating profile');
      }
    }
  };
</script>

<template>
  <div class="w-50 m-auto">
    <Form
      @submit="saveData"
      :validation-schema="schema"
      :validate-on-mount="false"
      :initial-values="initialValues"
      v-slot="{ isSubmitting }"
    >
      <div class="row mb-3">
        <div class="col">
          <label class="form-label">First Name</label>
          <Field class="form-control" name="firstName" />
          <ErrorMessage class="text-danger" name="firstName" />
        </div>
        <div class="col">
          <label class="form-label">Last Name</label>
          <Field class="form-control" name="lastName" />
          <ErrorMessage class="text-danger" name="lastName" />
        </div>
      </div>
      <div class="mb-3">
        <label class="form-label">Phone Number</label>
        <Field type="tel" class="form-control" name="phoneNum" />
        <ErrorMessage class="text-danger" name="phoneNum" />
      </div>
      <div class="mb-3">
        <label class="form-label">Birthday</label>
        <Field type="date" class="form-control" name="birthday" />
        <ErrorMessage class="text-danger" name="birthday" />
      </div>
      <div class="mb-3">
        <label class="form-label">Change Password</label>
        <div class="mb-3">
          <Field
            type="password"
            placeholder="New password"
            autocomplete="new-password"
            class="form-control"
            name="password"
          />
          <ErrorMessage class="text-danger" name="password" />
        </div>
        <div class="mb-3">
          <Field
            type="password"
            placeholder="Confirm new password"
            autocomplete="new-password"
            class="form-control"
            name="confirmPassword"
          />
          <ErrorMessage class="text-danger" name="confirmPassword" />
        </div>
      </div>
      <button type="submit" class="btn btn-primary">
        {{ isSubmitting ? 'Saving changes...' : 'Save changes' }}
      </button>
    </Form>
  </div>
</template>
